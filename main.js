const API_URL = "http://localhost:5274/api"; // cambiá si usás otro puerto

document.getElementById("loginForm").addEventListener("submit", async function (e) {
  e.preventDefault();

  const nombreUsuario = document.getElementById("nombreUsuario").value;
  const claveHash = document.getElementById("claveHash").value;

  const respuestaDiv = document.getElementById("respuesta");
  respuestaDiv.innerText = "Verificando...";

  try {
    const res = await fetch(`${API_URL}/auth/login`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ name: nombreUsuario, claveHash })
    });

    const data = await res.json();

    if (res.ok) {
      localStorage.setItem("token", data.token);
      respuestaDiv.innerText = "Login exitoso ✅";

      setTimeout(() => {
        window.location.href = "menu.html";
      }, 1000);
    } else {
      // Si la API devuelve un título con “invalid” o similar
      if (data.title && data.title.toLowerCase().includes("invalid")) {
        respuestaDiv.innerText = "Contraseña incorrecta ❌";
      } else {
        respuestaDiv.innerText = "Error: " + (data.title || "Credenciales inválidas");
      }
    }
  } catch (err) {
    console.error(err);
    respuestaDiv.innerText = "Error de conexión.";
  }
});