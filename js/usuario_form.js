<html lang="es">
<head>
<meta charset="UTF-8">
<title>Usuario</title>
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="bg-light">

<div class="container py-5">
  <h2 id="titulo">Nuevo Usuario</h2>

  <form id="usuarioForm">
    <input type="hidden" id="usuarioId">
    <div class="mb-3">
      <label for="name" class="form-label">Nombre</label>
      <input type="text" id="name" class="form-control" required>
    </div>

    <div class="mb-3">
      <label for="password" class="form-label">Contraseña</label>
      <input type="password" id="password" class="form-control">
    </div>

    <div class="form-check mb-3">
      <input class="form-check-input" type="checkbox" id="activo">
      <label class="form-check-label" for="activo">
        Activo
      </label>
    </div>

    <h5>Privilegios</h5>
    <div id="privilegiosList" class="mb-3"></div>

    <button type="submit" class="btn btn-primary">Guardar</button>
  </form>
</div>
