const API_URL = "https://gestionusuariosapi2025-drhmdmhcdsbzdnbq.canadacentral-01.azurewebsites.net/api";

// LOGIN
document.getElementById("loginForm")?.addEventListener("submit", async function (e) {
  e.preventDefault();

  const nombreUsuario = document.getElementById("nombreUsuario").value;
  const contraseña = document.getElementById("claveHash").value;

  const respuestaDiv = document.getElementById("respuesta");
  respuestaDiv.innerText = "Verificando...";

  try {
    const saltRes = await fetch(`${API_URL}/auth/salt?username=${encodeURIComponent(nombreUsuario)}`);
    if (!saltRes.ok) throw new Error("Usuario no encontrado");
    const saltData = await saltRes.json();
    const salt = saltData.salt;

    const hash = await calcularSHA256(contraseña + salt);

    const res = await fetch(`${API_URL}/auth/login`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ User: nombreUsuario, Password: hash })
    });

    const data = await res.json();

    if (res.ok) {
      localStorage.setItem("token", data.token);
      respuestaDiv.innerText = "Login exitoso ✅";

      setTimeout(() => {
        window.location.href = "../html/menu.html";
      }, 1000);
    } else {
      if (data.title && data.title.toLowerCase().includes("invalid")) {
        respuestaDiv.innerText = "Contraseña incorrecta ❌";
      } else {
        respuestaDiv.innerText = "Error: " + (data.title || "Credenciales inválidas");
      }
    }
  } catch (err) {
    console.error(err);
    respuestaDiv.innerText = "Error de conexión o usuario no válido.";
  }
});

async function calcularSHA256(text) {
  const encoder = new TextEncoder();
  const data = encoder.encode(text);
  const hashBuffer = await crypto.subtle.digest('SHA-256', data);
  const hashArray = Array.from(new Uint8Array(hashBuffer));
  const hashHex = hashArray.map(b => b.toString(16).padStart(2, '0')).join('');
  return hashHex;
}