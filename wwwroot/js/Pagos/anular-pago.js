document.addEventListener("DOMContentLoaded", function () {
  const confirmarBtn = document.getElementById("confirmarAnulacionBtn");
  const form = document.getElementById("anularPagoForm");

  // Obtenemos el número de pago desde el HTML directamente (no del form)
  const numeroPago =
    document.querySelector("dd[data-pago-numero]")?.textContent?.trim() ||
    "desconocido";

  confirmarBtn.addEventListener("click", function () {
    const formData = new FormData(form);
    const formObject = Object.fromEntries(formData);

    // Validación personalizada
    const errores = [];

    if (!formObject.IdUsuarioAnula) {
      errores.push("Debés seleccionar el usuario que anula el pago.");
    }

    if (
      !formObject.MotivoAnulacion ||
      formObject.MotivoAnulacion.trim().length < 3
    ) {
      errores.push("Debés completar el motivo de la anulación.");
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

    Swal.fire({
      title: "¿Estás seguro?",
      html: `
        <p>Esta acción <strong>anulará</strong> el pago <strong>#${numeroPago}</strong>.</p>
        <p><strong>Motivo:</strong> ${formObject.MotivoAnulacion}</p>
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

        // Armamos el objeto JSON que espera el backend
        const payload = {
          IdPago: parseInt(formObject.IdPago),
          IdUsuarioAnula: parseInt(formObject.IdUsuarioAnula),
          MotivoAnulacion: formObject.MotivoAnulacion.trim(),
          FechaAnulacion: new Date(formObject.FechaAnulacion).toISOString(),
        };

        fetch(anularPagoUrl, {
          method: "POST",
          headers: {
            "X-Requested-With": "XMLHttpRequest",
            "Content-Type": "application/json",
          },
          body: JSON.stringify(payload),
        })
          .then((res) => {
            if (!res.ok) {
              throw new Error("Error en el servidor para anular el pago.");
            }
            return res.json();
          })
          .then((data) => {
            if (data.success) {
              Swal.fire({
                title: "¡Éxito!",
                text: "El pago ha sido anulado correctamente.",
                icon: "success",
                confirmButtonText: "Volver al listado",
              }).then(() => {
                window.location.href = data.redirectUrl || "/Pagos";
              });
            } else {
              throw new Error(data.message || "No se pudo anular el pago.");
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
