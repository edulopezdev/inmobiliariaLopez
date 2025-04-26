$(document).ready(function () {
  $("#logoutForm").on("submit", function (e) {
    e.preventDefault(); // Evita que el formulario se envíe inmediatamente

    // Mostrar el SweetAlert2 de confirmación
    Swal.fire({
      title: "¿Estás seguro?",
      text: "¿Quieres cerrar sesión?",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Sí, cerrar sesión",
      cancelButtonText: "No, cancelar",
    }).then((result) => {
      if (result.isConfirmed) {
        // Si el usuario confirma, enviamos el formulario
        this.submit();
      }
    });
  });
});
