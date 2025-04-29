document.addEventListener("DOMContentLoaded", function () {
  const confirmarBtn = document.getElementById("confirmarAnulacionBtn");
  const form = document.getElementById("anularPagoForm");

  // Obtenemos el número de pago desde el HTML directamente (no del form)
  const numeroPago =
    document.querySelector("dd[data-pago-numero]")?.textContent?.trim() ||
    "desconocido";

  console.log("Número de pago:", numeroPago); // Verifica que el número de pago se obtiene correctamente

  confirmarBtn.addEventListener("click", function () {
    const formData = new FormData(form);
    const formObject = Object.fromEntries(formData);

    console.log("Datos del formulario:", formObject); // Verifica que los datos del formulario se están leyendo correctamente

    // Validación personalizada
    const errores = [];

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
      console.log("Errores en los datos de anulación:", errores); // Muestra los errores si los hay
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
          MotivoAnulacion: formObject.MotivoAnulacion.trim(),
          FechaAnulacion: new Date(formObject.FechaAnulacion).toISOString(),
        };

        console.log("Datos que se enviarán al servidor:", payload); // Verifica los datos antes de enviarlos

        fetch(anularPagoUrl, {
          method: "POST",
          headers: {
            "X-Requested-With": "XMLHttpRequest",
            "Content-Type": "application/json",
          },
          body: JSON.stringify(payload),
        })
          .then((res) => {
            console.log("Respuesta del servidor:", res); // Verifica la respuesta del servidor
            if (!res.ok) {
              throw new Error("Error en el servidor para anular el pago.");
            }
            return res.json();
          })
          .then((data) => {
            console.log("Datos de la respuesta:", data); // Muestra los datos de la respuesta
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
            console.error("Error en la anulación del pago:", error); // Muestra el error en caso de fallo
            Swal.fire("Error", error.message, "error");
          })
          .finally(() => {
            confirmarBtn.disabled = false;
          });
      }
    });
  });
});
