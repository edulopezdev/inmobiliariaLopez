document.addEventListener("DOMContentLoaded", function () {
  const confirmarBtn = document.getElementById("confirmarAnulacionBtn");
  const form = document.getElementById("anularContratoForm");

  confirmarBtn.addEventListener("click", function () {
    const formData = new FormData(form);
    const formObject = Object.fromEntries(formData);

    // Validación personalizada
    const errores = [];

    // Validar usuario
    if (!formObject.IdUsuarioAnula) {
      errores.push("Debés seleccionar el usuario que anula el contrato.");
    }

    // Validar fecha rescisión
    if (!formObject.FechaRescision) {
      errores.push("Debés ingresar la fecha de rescisión.");
    } else {
      const fechaIngresada = new Date(formObject.FechaRescision);
      const hoy = new Date();
      hoy.setHours(0, 0, 0, 0); // Limpiar hora para comparar solo fecha

      if (fechaIngresada < hoy) {
        errores.push(
          "La fecha de rescisión no puede ser anterior a la fecha actual."
        );
      }
    }

    // Validar observaciones
    if (
      !formObject.Observaciones ||
      formObject.Observaciones.trim().length < 3
    ) {
      errores.push("Debés completar el motivo de anulación.");
    }

    if (errores.length > 0) {
      Swal.fire({
        title: "Faltan datos",
        html: errores.map((e) => `<p>• ${e}</p>`).join(""),
        icon: "warning",
        confirmButtonText: "Ok",
      });
      return;
    }

    // Confirmación final
    Swal.fire({
      title: "¿Estás seguro?",
      html: `
        <p>Esta acción <strong>anulará</strong> el contrato <strong>#${formObject.IdContrato}</strong>.</p>
        <p><strong>Motivo:</strong> ${formObject.Observaciones}</p>
        <p>¿Deseás continuar?</p>
      `,
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#dc3545",
      cancelButtonColor: "#6c757d",
      confirmButtonText: "Sí, anular",
      cancelButtonText: "Cancelar",
    }).then((result) => {
      if (result.isConfirmed) {
        confirmarBtn.disabled = true;

        Swal.fire({
          title: "Procesando...",
          allowOutsideClick: false,
          didOpen: () => Swal.showLoading(),
        });

        fetch(anularContratoUrl, {
          method: "POST",
          headers: {
            "X-Requested-With": "XMLHttpRequest",
            "Content-Type": "application/json",
          },
          body: JSON.stringify(formObject),
        })
          .then((res) => {
            if (!res.ok)
              throw new Error("Error en el servidor para anular el contrato.");
            return res.json();
          })
          .then((data) => {
            if (data.success) {
              Swal.fire({
                title: "¡Éxito!",
                text: "El contrato ha sido anulado correctamente.",
                icon: "success",
                confirmButtonText: "Volver al listado",
              }).then(() => {
                window.location.href = data.redirectUrl || "/Contratos";
              });
            } else {
              throw new Error(data.message || "No se pudo anular el contrato.");
            }
          })
          .catch((error) => {
            Swal.fire("Error", error.message, "error");
          })
          .finally(() => {
            confirmarBtn.disabled = false;
          });
      }
    });
  });
});
