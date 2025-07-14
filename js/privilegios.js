document.addEventListener('DOMContentLoaded', () => {
  if (document.getElementById("listaPrivilegios")) {
    listarPrivilegios();
  }
});

function listarPrivilegios() {
  fetch(`${API_URL}/Privilegios`)
    .then(r => r.json())
    .then(data => {
      const ul = document.getElementById('listaPrivilegios');
      ul.innerHTML = '';
      data.forEach(p => {
        const li = document.createElement('li');
        li.className = 'list-group-item';
        li.textContent = `${p.id} - ${p.descripcion}`;
        ul.appendChild(li);
      });
    });
}

function crearPrivilegio() {
  const desc = document.getElementById('nuevoPrivilegio').value.trim();
  if (!desc) return alert('Escribe una descripción');
  fetch(`${API_URL}/Privilegios`, {
    method: 'POST',
    headers: {'Content-Type': 'application/json'},
    body: JSON.stringify({ descripcion: desc })
  })
  .then(() => {
    document.getElementById('nuevoPrivilegio').value = '';
    listarPrivilegios();
  });
}

function asignarPrivilegio() {
  const usuarioId = parseInt(document.getElementById('usuarioId').value);
  const privilegioId = parseInt(document.getElementById('privilegioId').value);
  if (!usuarioId || !privilegioId) return alert('Completa ambos campos');

  fetch(`${API_URL}/UsuariosPrivilegios`, {
    method: 'POST',
    headers: {'Content-Type': 'application/json'},
    body: JSON.stringify({ usuarioId, privilegioId })
  })
  .then(() => alert('Privilegio asignado'));
}

function verPrivilegiosUsuario() {
  const usuarioId = parseInt(document.getElementById('usuarioConsultaId').value);
  if (!usuarioId) return alert('Escribe el ID del usuario');

  fetch(`${API_URL}/UsuariosPrivilegios/${usuarioId}`)
    .then(r => r.json())
    .then(data => {
      const ul = document.getElementById('listaPrivilegiosUsuario');
      ul.innerHTML = '';
      data.forEach(p => {
        const li = document.createElement('li');
        li.className = 'list-group-item';
        li.textContent = `${p.id} - ${p.descripcion}`;
        ul.appendChild(li);
      });
    });
}