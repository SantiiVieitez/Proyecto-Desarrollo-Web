function getQueryParam(name) {
  const urlParams = new URLSearchParams(window.location.search);
  return urlParams.get(name);
}

document.addEventListener("DOMContentLoaded", async () => {
  const container = document.getElementById("privilegiosContainer");
  const usuarioId = getQueryParam("id");
  const titulo = document.querySelector("h2");

  if (usuarioId) {
    titulo.textContent = "Editar Usuario";
  }

  try {
    const resPrivs = await fetch(`${API_URL}/privilegios`);
    if (!resPrivs.ok) throw new Error("No se pudieron cargar los privilegios");

    const privilegios = await resPrivs.json();

    privilegios.forEach(p => {
      const div = document.createElement("div");
      div.classList.add("form-check");

      div.innerHTML = `
        <input type="checkbox" class="form-check-input privilegio-checkbox" 
               value="${p.id}" id="priv-${p.id}">
        <label for="priv-${p.id}" class="form-check-label">${p.descripcion}</label>
      `;

      container.appendChild(div);
    });

    if (usuarioId) {
      const resUser = await fetch(`${API_URL}/usuarios/${usuarioId}`);
      if (!resUser.ok) throw new Error("Usuario no encontrado");

      const usuario = await resUser.json();
      document.getElementById("nombreUsuario").value = usuario.name;
      document.getElementById("activo").checked = usuario.activo;

      usuario.privilegios.forEach(priv => {
        const checkbox = document.getElementById(`priv-${priv.id}`);
        if (checkbox) checkbox.checked = true;
      });
    }

  } catch (err) {
    console.error(err);
    alert("❌ Error al cargar datos: " + err.message);
  }
});

document.getElementById("guardarBtn").addEventListener("click", async () => {
  const usuarioId = getQueryParam("id");

  const nombre = document.getElementById("nombreUsuario").value.trim();
  const password = document.getElementById("clave").value.trim();
  const activo = document.getElementById("activo").checked;

  if (!nombre) {
    alert("El nombre de usuario es obligatorio");
    return;
  }

  const privilegiosSeleccionados = [];
    document.querySelectorAll(".privilegio-checkbox:checked").forEach(cb => {
      privilegiosSeleccionados.push(parseInt(cb.value, 10));
    });

    const payload = {
      name: nombre,
      activo: activo,
      salt: "abc",
      privilegiosIds: privilegiosSeleccionados
    };

    if (password) {
      payload.claveHash = password;
    }

    if (usuarioId) {
      payload.id = parseInt(usuarioId, 10);
    }

  try {
    let res;
    if (usuarioId) {
      res = await fetch(`${API_URL}/usuarios/${usuarioId}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload)
      });
    } else {
      res = await fetch(`${API_URL}/usuarios`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload)
      });
    }

    if (res.ok) {
      alert("✅ Usuario guardado correctamente");
      window.location.href =
        `${window.location.origin}${window.location.pathname.replace(/\/[^\/]*$/, '/') + 'usuarios.html'}`;
    } else {
      const error = await res.json();
      console.error(error);
      alert("❌ Error al guardar usuario. Revisa los datos enviados.");
    }
  } catch (err) {
    console.error(err);
    alert("❌ Error de conexión: " + err.message);
  }
});