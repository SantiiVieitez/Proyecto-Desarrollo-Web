const API_URL = "https://gestionusuariosapi2025-drhmdmhcdsbzdnbq.canadacentral-01.azurewebsites.net/api";

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

    try {
      if (insumo.id == 0) {
        await fetch(`${API_URL}/insumos`, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(insumo)
        });
      } else {
        await fetch(`${API_URL}/insumos/${insumo.id}`, {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(insumo)
        });
      }

      e.target.reset();
      document.getElementById('insumoId').value = '';
      cargarInsumos();

    } catch (err) {
      console.error("Error al guardar insumo:", err);
      alert("Ocurrió un error al guardar el insumo.");
    }
  });

  document.getElementById('btnclean')?.addEventListener('click', () => {
    document.getElementById('insumoForm').reset();
    document.getElementById('insumoId').value = '';
  });
});

async function cargarInsumos() {
  try {
    const res = await fetch(`${API_URL}/insumos`);
    if (!res.ok) throw new Error("No se pudieron cargar los insumos");
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

  } catch (err) {
    console.error("Error al cargar insumos:", err);
    alert("Ocurrió un error al cargar los insumos.");
  }
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