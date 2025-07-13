document.getElementById("insumoForm").addEventListener("submit", function (e) {
  e.preventDefault();

  const id = document.getElementById("id").value;
  const nombre = document.getElementById("nombre").value;
  const descripcion = document.getElementById("descripcion").value;
  const marca = document.getElementById("marca").value;
  const stock = document.getElementById("stock").value;
  const precio = document.getElementById("precio").value;
  const codigo = document.getElementById("codigo").value;

  const table = document.getElementById("insumosTable");
  const row = table.insertRow();

  row.innerHTML = `
    <td>${id}</td>
    <td>${nombre}</td>
    <td>${descripcion}</td>
    <td>${marca}</td>
    <td>${stock}</td>
    <td>${precio}</td>
    <td>${codigo}</td>
  `;

  document.getElementById("insumoForm").reset();
});