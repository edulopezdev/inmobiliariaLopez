document.addEventListener("DOMContentLoaded", function () {
  const form = document.getElementById("editContratoForm");

  form.addEventListener("submit", async function (event) {
    event.preventDefault();

    const formData = new FormData(form);
    const errores = [];

    // Validaciones del formulario
    if (!formData.get("IdUsuarioCrea"))
      errores.push("Debés seleccionar un usuario del sistema.");
    if (!formData.get("IdInmueble"))
      errores.push("Debés seleccionar un inmueble.");
    if (!formData.get("IdInquilino"))
      errores.push("Debés seleccionar un inquilino.");

    const fechaInicioStr = formData.get("FechaInicio");
    const fechaFinStr = formData.get("FechaFin");

    if (!fechaInicioStr) {
      errores.push("Debés ingresar la fecha de inicio.");
    } else {
      const fechaInicio = new Date(fechaInicioStr);
      const hoy = new Date();
      hoy.setHours(0, 0, 0, 0);
      if (fechaInicio < hoy) {
        errores.push("La fecha de inicio no puede ser anterior a hoy.");
      }
    }

    if (!fechaFinStr) {
      errores.push("Debés ingresar la fecha de fin.");
    } else if (fechaInicioStr) {
      const fechaInicio = new Date(fechaInicioStr);
      const fechaFin = new Date(fechaFinStr);
      if (fechaFin <= fechaInicio) {
        errores.push("La fecha de fin debe ser posterior a la de inicio.");
      }
    }

    const monto = parseFloat(formData.get("MontoMensual"));
    if (!formData.get("MontoMensual")) {
      errores.push("Debés ingresar el monto mensual.");
    } else if (isNaN(monto) || monto <= 0) {
      errores.push("El monto mensual debe ser un número positivo.");
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

    // Verificar disponibilidad de fechas (excluyendo este contrato)
    const idContrato = formData.get("IdContrato");
    const idInmueble = formData.get("IdInmueble");
    const fechaInicioISO = new Date(fechaInicioStr).toISOString();
    const fechaFinISO = new Date(fechaFinStr).toISOString();

    try {
      const disponibilidadResponse = await fetch(
        "/Contratos/VerificarDisponibilidad",
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            IdContrato: idContrato,
            IdInmueble: idInmueble,
            FechaInicio: fechaInicioISO,
            FechaFin: fechaFinISO,
          }),
        }
      );

      const disponibilidadData = await disponibilidadResponse.json();

      if (!disponibilidadData.success) {
        Swal.fire({
          title: "Fechas ocupadas",
          html:
            disponibilidadData.message ||
            "El inmueble no está disponible en esas fechas.",
          icon: "warning",
          confirmButtonText: "Ok",
        });
        return;
      }

      // Confirmación antes de enviar
      const confirmacion = await Swal.fire({
        title: "¿Estás seguro?",
        text: "¿Deseás guardar los cambios del contrato?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Sí, guardar",
        cancelButtonText: "Cancelar",
      });

      if (!confirmacion.isConfirmed) return;

      // Mostrar cargando
      Swal.fire({
        title: "Guardando...",
        text: "Por favor espera mientras guardamos los cambios.",
        icon: "info",
        allowOutsideClick: false,
        didOpen: () => Swal.showLoading(),
      });

      const response = await fetch('@Url.Action("Edit")', {
        method: "POST",
        body: formData,
        headers: {
          "X-Requested-With": "XMLHttpRequest",
        },
      });

      const data = await response.json();
      Swal.close();

      if (data.success) {
        Swal.fire({
          title: "¡Éxito!",
          text: "Contrato modificado correctamente.",
          icon: "success",
          confirmButtonText: "Ok",
        }).then(() => {
          window.location.href = data.redirectUrl;
        });
      } else {
        Swal.fire({
          title: "¡Error!",
          html: data.message || "Ocurrió un error al guardar el contrato.",
          icon: "error",
          confirmButtonText: "Ok",
        });
      }
    } catch (error) {
      console.error("Error en la petición AJAX:", error);
      Swal.fire({
        title: "¡Error inesperado!",
        text: "No se pudo completar la operación. Intentá nuevamente.",
        icon: "error",
        confirmButtonText: "Ok",
      });
    }
  });
});
