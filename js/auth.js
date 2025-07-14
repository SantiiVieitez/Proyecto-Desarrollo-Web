// auth.js
function checkAuth() {
  const token = localStorage.getItem("token");

  if (!token) {
    // Si no hay token, lo enviamos al login
    alert("Debes iniciar sesión para acceder.");
    window.location.href = "index.html";
  }

  // Opcional: validar el token con el backend para asegurarte que no esté expirado
}