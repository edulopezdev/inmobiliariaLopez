document.addEventListener("DOMContentLoaded", function () {
  const form = document.getElementById("createContratoForm");

  form.addEventListener("submit", async function (event) {
    event.preventDefault(); // Evita el envío tradicional

    const formData = new FormData(form);
    const errores = [];

    // Validaciones del formulario
    if (!formData.get("IdInmueble"))
      errores.push("Debés seleccionar un inmueble.");
    if (!formData.get("IdInquilino"))
      errores.push("Debés seleccionar un inquilino.");

    const fechaInicioStr = formData.get("FechaInicio");
    const fechaFinStr = formData.get("FechaFin");

    if (!fechaInicioStr) {
      errores.push("Debés ingresar la fecha de inicio.");
    } else {
      // Obtener la fecha de inicio
      const fechaInicio = new Date(fechaInicioStr);

      // Ajustamos la fecha de inicio para asegurarnos de que es a las 00:00:00
      fechaInicio.setHours(0, 0, 0, 0);

      // Obtener la fecha de hoy sin hora
      const hoy = new Date();
      hoy.setHours(0, 0, 0, 0);

      // Comparación de fechas sin hora
      if (fechaInicio < hoy) {
        errores.push(
          "La fecha de inicio no puede ser anterior a la fecha actual."
        );
      }
    }

    if (!fechaFinStr) {
      errores.push("Debés ingresar la fecha de fin.");
    } else if (fechaInicioStr) {
      const fechaInicio = new Date(fechaInicioStr);
      const fechaFin = new Date(fechaFinStr);
      if (fechaFin <= fechaInicio) {
        errores.push(
          "La fecha de fin debe ser posterior a la fecha de inicio."
        );
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

    // Verificar disponibilidad de fechas
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
        text: "¿Deseás crear este contrato?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Sí, crear",
        cancelButtonText: "Cancelar",
      });

      if (!confirmacion.isConfirmed) return;

      Swal.fire({
        title: "Creando...",
        text: "Por favor esperá mientras se crea el contrato.",
        icon: "info",
        allowOutsideClick: false,
        didOpen: () => Swal.showLoading(),
      });

      // Enviar el formulario
      const createResponse = await fetch(createContratoUrl, {
        method: "POST",
        body: formData,
        headers: { "X-Requested-With": "XMLHttpRequest" },
      });

      const createData = await createResponse.json();
      Swal.close();

      if (createData.success) {
        Swal.fire({
          title: "¡Éxito!",
          text: "Contrato creado correctamente.",
          icon: "success",
          confirmButtonText: "Ok",
        }).then(() => {
          window.location.href = createData.redirectUrl;
        });
      } else {
        Swal.fire({
          title: "¡Error!",
          html: createData.message || "Ocurrió un error al crear el contrato.",
          icon: "error",
          confirmButtonText: "Ok",
        });
      }
    } catch (error) {
      Swal.fire({
        title: "¡Error inesperado!",
        text: "No se pudo completar la operación. Intentá nuevamente.",
        icon: "error",
        confirmButtonText: "Ok",
      });
    }
  });
});
