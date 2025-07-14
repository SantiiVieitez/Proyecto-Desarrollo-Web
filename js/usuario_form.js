const API_URL = "https://gestionusuariosapi2025-drhmdmhcdsbzdnbq.canadacentral-01.azurewebsites.net/api";

// Cargar privilegios en la página
document.addEventListener("DOMContentLoaded", async () => {
  const container = document.getElementById("privilegiosContainer");

  try {
    const res = await fetch(`${API_URL}/privilegios`);
    const privilegios = await res.json();

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
  } catch (err) {
    console.error(err);
    alert("Error al cargar privilegios");
  }
});

document.getElementById("guardarBtn").addEventListener("click", async () => {
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
    claveHash: password,
    activo: activo,
    salt: "abc",
    privilegios: privilegiosSeleccionados
  };

  try {
    const res = await fetch(`${API_URL}/usuarios`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(payload)
    });

    if (res.ok) {
      alert("✅ Usuario guardado correctamente");
      window.location.href = "usuarios.html";
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