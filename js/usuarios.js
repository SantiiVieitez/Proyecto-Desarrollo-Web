async function cargarUsuarios() {
  const res = await fetch(`${API_URL}/usuarios`);
  const usuarios = await res.json();
  const tbody = document.getElementById("usuariosTable");
  tbody.innerHTML = "";
  usuarios.forEach(u => {
    tbody.innerHTML += `
      <tr>
        <td>${u.id}</td>
        <td>${u.name}</td>
        <td>${u.activo ? '✅' : '❌'}</td>
        <td>
          <button class="btn btn-sm btn-warning" onclick="editar(${u.id})">Editar</button>
        </td>
      </tr>`;
  });
}

function editar(id) {
  location.href = `${window.location.origin}${window.location.pathname.replace(/\/[^\/]*$/, '/') + `usuario_form.html?id=${id}`}`;
}

function buscar() {
  const query = document.getElementById("buscador").value.toLowerCase();
  [...document.querySelectorAll("#usuariosTable tr")].forEach(row => {
    row.style.display = row.innerText.toLowerCase().includes(query) ? "" : "none";
  });
}

document.addEventListener("DOMContentLoaded", cargarUsuarios);