document.addEventListener('DOMContentLoaded', () => {
  cargarInsumos();
  verificarPrivilegios();

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

    const token = localStorage.getItem('token');

    try {
      const res = await fetch(`${API_URL}/insumos${insumo.id == 0 ? '' : '/' + insumo.id}`, {
        method: insumo.id == 0 ? 'POST' : 'PUT',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(insumo)
      });

      if (!res.ok) {
        const data = await res.text();
        throw new Error(data || "Error al guardar");
      }

      e.target.reset();
      document.getElementById('insumoId').value = '';
      cargarInsumos();

    } catch (err) {
      console.error("Error al guardar insumo:", err);
      alert("No tienes permisos para crear o modificar insumos.");
    }
  });

  document.getElementById('btnclean')?.addEventListener('click', () => {
    document.getElementById('insumoForm').reset();
    document.getElementById('insumoId').value = '';
  });
});

async function verificarPrivilegios() {
  const token = localStorage.getItem('token');
  if (!token) return;

  const payload = JSON.parse(atob(token.split('.')[1]));

  const privilegios = payload.Privilegio || []; // OJO: mayúscula
  const puedeCrear = privilegios.includes('CrearInsumo');
  const puedeModificar = privilegios.includes('ModificarInsumo');

  const btnGuardar = document.querySelector('#insumoForm button[type="submit"]');
  if (!puedeCrear && !puedeModificar) {
    btnGuardar.disabled = true;
    btnGuardar.title = "No tienes permiso para crear/editar insumos.";
  }

  // Guardamos en window para reutilizar en la tabla
  window.tienePermisoCrearInsumo = puedeCrear;
  window.tienePermisoModificarInsumo = puedeModificar;
}

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
          <button class="btn btn-sm btn-warning me-1 editar-btn" ${
            window.tienePermisoModificarInsumo ? '' : 'disabled title="Sin permiso"'
          } onclick='editarInsumo(${JSON.stringify(insumo)})'>Editar</button>
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
  if (!window.tienePermisoModificarInsumo) {
    alert("No tienes permisos para editar insumos.");
    return;
  }

  document.getElementById('insumoId').value = insumo.id;
  document.getElementById('nombre').value = insumo.nombre;
  document.getElementById('descripcion').value = insumo.descripcion;
  document.getElementById('marca').value = insumo.marca;
  document.getElementById('stock').value = insumo.stock;
  document.getElementById('precio').value = insumo.precio;
  document.getElementById('codigo').value = insumo.codigo;
}
