const apiUrl = 'http://localhost:5274/api/insumos';

document.addEventListener('DOMContentLoaded', () => {
  cargarInsumos();

  document.getElementById('insumoForm').addEventListener('submit', async e => {
    e.preventDefault();

    const insumo = {
      id: document.getElementById('insumoId').value || 0,
      nombre: document.getElementById('nombre').value,
      descripcion: document.getElementById('descripcion').value,
      marca: document.getElementById('marca').value,
      stock: parseInt(document.getElementById('stock').value),
      precio: parseFloat(document.getElementById('precio').value),
      codigo: document.getElementById('codigo').value
    };

    if (insumo.id == 0) {
      await fetch(apiUrl, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(insumo)
      });
    } else {
      await fetch(`${apiUrl}/${insumo.id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(insumo)
      });
    }

    e.target.reset();
    document.getElementById('insumoId').value = '';
    cargarInsumos();
  });
});

async function cargarInsumos() {
  const res = await fetch(apiUrl);
  const insumos = await res.json();
  const tbody = document.getElementById('insumosTable');
  tbody.innerHTML = '';

  insumos.forEach(insumo => {
    const row = document.createElement('tr');
    row.innerHTML = `
      <td>${insumo.id}</td>
      <td>${insumo.nombre}</td>
      <td>${insumo.descripcion}</td>
      <td>${insumo.marca}</td>
      <td>${insumo.stock}</td>
      <td>$${insumo.precio.toFixed(2)}</td>
      <td>${insumo.codigo}</td>
      <td>
        <button class="btn btn-sm btn-warning me-1" onclick='editarInsumo(${JSON.stringify(insumo)})'>Editar</button>
      </td>
    `;
    tbody.appendChild(row);
  });
}

function editarInsumo(insumo) {
  document.getElementById('insumoId').value = insumo.id;
  document.getElementById('nombre').value = insumo.nombre;
  document.getElementById('descripcion').value = insumo.descripcion;
  document.getElementById('marca').value = insumo.marca;
  document.getElementById('stock').value = insumo.stock;
  document.getElementById('precio').value = insumo.precio;
  document.getElementById('codigo').value = insumo.codigo;
}