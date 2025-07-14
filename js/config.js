// Cambia esto a "production" o "development" según donde estés trabajando
const ENV = "development";
const API_URLS = {
  development: "http://localhost:5274/api",
  production: "https://gestionusuariosapi2025-drhmdmhcdsbzdnbq.canadacentral-01.azurewebsites.net/api"
};

const API_URL = API_URLS[ENV];