const API_URL = "https://gestionusuariosapi2025-drhmdmhcdsbzdnbq.canadacentral-01.azurewebsites.net/api";

// helper para obtener query params
function getQueryParam(name) {
  const urlParams = new URLSearchParams(window.location.search);
  return urlParams.get(name);
}

// Cargar privilegios y (opcionalmente) datos del usuario
document.addEventListener("DOMContentLoaded", async () => {
  const container = document.getElementById("privilegiosContainer");
  const usuarioId = getQueryParam("id");
  const titulo = document.querySelector("h2");

  if (usuarioId) {
    titulo.textContent = "Editar Usuario";
  }

  try {
    const resPrivs = await fetch(`${API_URL}/privilegios`);
    const privilegios = await resPrivs.json();

    privilegios.forEach(p => {
      const div = document.createElement("div");
      div.classList.add("form-check");

      div.innerHTML = `
        <input type="checkbox" class="form-check-input privilegio-checkbox" 
               value="${p.id}" data-descripcion="${p.descripcion}" id="priv-${p.id}">
        <label for="priv-${p.id}" class="form-check-label">${p.descripcion}</label>
      `;

      container.appendChild(div);
    });

    if (usuarioId) {
      // cargar datos del usuario
      const resUser = await fetch(`${API_URL}/usuarios/${usuarioId}`);
      if (!resUser.ok) throw new Error("Usuario no encontrado");

      const usuario = await resUser.json();
      document.getElementById("nombreUsuario").value = usuario.name;
      document.getElementById("activo").checked = usuario.activo;

      // marcar privilegios
      usuario.privilegios.forEach(priv => {
        const checkbox = document.getElementById(`priv-${priv.id}`);
        if (checkbox) checkbox.checked = true;
      });
    }

  } catch (err) {
    console.error(err);
    alert("Error al cargar datos");
  }
});

// Guardar
document.getElementById("guardarBtn").addEventListener("click", async () => {
  const usuarioId = getQueryParam("id");

  const nombre = document.getElementById("nombreUsuario").value;
  const password = document.getElementById("clave").value;
  const activo = document.getElementById("activo").checked;

  const privilegiosSeleccionados = [];
  document.querySelectorAll(".privilegio-checkbox:checked").forEach(cb => {
    privilegiosSeleccionados.push({
      id: parseInt(cb.value),
      descripcion: cb.dataset.descripcion
    });
  });

  const payload = {
    name: nombre,
    activo: activo,
    salt: "abc",
    privilegios: privilegiosSeleccionados
  };

  if (password.trim()) {
    payload.claveHash = password;
  }

  if (usuarioId) {
    payload.id = parseInt(usuarioId); // 👈 agrega el id al body
  }

  try {
    let res;
    if (usuarioId) {
      // PUT para actualizar
      res = await fetch(`${API_URL}/usuarios/${usuarioId}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload)
      });
    } else {
      // POST para crear
      res = await fetch(`${API_URL}/usuarios`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload)
      });
    }

    if (res.ok) {
      alert("✅ Usuario guardado correctamente");
      window.location.href = `${window.location.origin}${window.location.pathname.replace(/\/[^\/]*$/, '/') + 'usuarios.html'}`;
    } else {
      const error = await res.json();
      console.error(error);
      alert("❌ Error al guardar usuario");
    }
  } catch (err) {
    console.error(err);
    alert("❌ Error de conexión");
  }
});