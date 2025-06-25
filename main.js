const API_URL = "http://localhost:5274/api"; // cambiá si usás otro puerto

document.getElementById("loginForm").addEventListener("submit", async function (e) {
  e.preventDefault();

  const nombreUsuario = document.getElementById("nombreUsuario").value;
  const claveHash = document.getElementById("claveHash").value;

  const res = await fetch(`${API_URL}/auth/login`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({ name: nombreUsuario, claveHash }) // 👈 cambio clave
  });

  const data = await res.json();

  if (res.ok) {
    localStorage.setItem("token", data.token);
    document.getElementById("respuesta").innerText = "Login exitoso ✅";
  } else {
    document.getElementById("respuesta").innerText = "Error: " + (data.title || "Credenciales inválidas");
  }
});