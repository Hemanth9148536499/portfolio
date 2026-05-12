// ──────────────────────────────────────────────────────────────────
//  api.js  — All calls to the .NET REST API
// ──────────────────────────────────────────────────────────────────

const API = {
  base: () => CONFIG.API_BASE_URL,

  async get(path) {
    try {
      const res = await fetch(`${this.base()}${path}`);
      if (!res.ok) throw new Error(`HTTP ${res.status}`);
      return await res.json();
    } catch (e) {
      console.warn(`[API] GET ${path} failed:`, e.message);
      return null;
    }
  },

  async post(path, body) {
    const res = await fetch(`${this.base()}${path}`, {
      method:  "POST",
      headers: { "Content-Type": "application/json" },
      body:    JSON.stringify(body)
    });
    if (!res.ok) {
      const err = await res.json().catch(() => ({}));
      throw new Error(err.error || `HTTP ${res.status}`);
    }
    return await res.json();
  },

  projects:   { getAll: (f) => API.get(`/api/projects${f ? "?featured=true" : ""}`) },
  skills:     { getAll: (cat) => API.get(`/api/skills${cat ? `?category=${cat}` : ""}`) },
  experience: { getAll: () => API.get("/api/experience") },
  contact:    { send: (data) => API.post("/api/contact", data) }
};
