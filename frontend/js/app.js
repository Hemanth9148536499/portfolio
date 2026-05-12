// ──────────────────────────────────────────────────────────────────
//  app.js  — Portfolio rendering engine
// ──────────────────────────────────────────────────────────────────

document.addEventListener("DOMContentLoaded", async () => {

  // ── Populate owner info from config ─────────────────────────────
  const o = CONFIG.OWNER;
  document.querySelectorAll("[data-owner-name]").forEach(el   => el.textContent = o.name);
  document.querySelectorAll("[data-owner-title]").forEach(el  => el.textContent = o.title);
  document.querySelectorAll("[data-owner-tagline]").forEach(el=> el.textContent = o.tagline);
  document.querySelectorAll("[data-owner-email]").forEach(el  => { el.textContent = o.email; el.href = `mailto:${o.email}`; });
  document.querySelectorAll("[data-owner-github]").forEach(el => el.href = o.github);
  document.querySelectorAll("[data-owner-linkedin]").forEach(el => el.href = o.linkedin);
  document.title = `${o.name} — Portfolio`;

  // ── Nav: active link on scroll ───────────────────────────────────
  const sections = document.querySelectorAll("section[id]");
  const navLinks = document.querySelectorAll(".nav-link");
  const observer = new IntersectionObserver(entries => {
    entries.forEach(e => {
      if (e.isIntersecting) {
        navLinks.forEach(l => l.classList.remove("active"));
        const link = document.querySelector(`.nav-link[href="#${e.target.id}"]`);
        if (link) link.classList.add("active");
      }
    });
  }, { threshold: 0.4 });
  sections.forEach(s => observer.observe(s));

  // ── Mobile nav toggle ────────────────────────────────────────────
  document.getElementById("navToggle")?.addEventListener("click", () => {
    document.querySelector(".nav-links")?.classList.toggle("open");
  });

  // ── Load all data in parallel ────────────────────────────────────
  const [projects, skills, experience] = await Promise.all([
    API.projects.getAll(),
    API.skills.getAll(),
    API.experience.getAll()
  ]);

  renderProjects(projects);
  renderSkills(skills);
  renderExperience(experience);

  // ── Animate elements on scroll ───────────────────────────────────
  const revealObs = new IntersectionObserver(entries => {
    entries.forEach(e => {
      if (e.isIntersecting) {
        e.target.classList.add("revealed");
        revealObs.unobserve(e.target);
      }
    });
  }, { threshold: 0.1 });
  document.querySelectorAll(".reveal").forEach(el => revealObs.observe(el));
});

// ── Projects ──────────────────────────────────────────────────────
function renderProjects(projects) {
  const grid = document.getElementById("projectsGrid");
  if (!grid) return;

  if (!projects || !projects.length) {
    grid.innerHTML = `<p class="empty-state">No projects found. Make sure your API is running.</p>`;
    return;
  }

  grid.innerHTML = projects.map((p, i) => {
    const tags = p.techStack.split(",").map(t =>
      `<span class="tag">${t.trim()}</span>`).join("");
    return `
    <article class="project-card reveal" style="animation-delay:${i * 80}ms">
      ${p.isFeatured ? '<span class="badge">Featured</span>' : ""}
      <h3 class="project-title">${p.title}</h3>
      <p class="project-desc">${p.description}</p>
      <div class="tag-list">${tags}</div>
      <div class="project-links">
        ${p.githubUrl ? `<a href="${p.githubUrl}" target="_blank" rel="noopener" class="btn-ghost">
          <svg viewBox="0 0 24 24" fill="currentColor" width="16" height="16"><path d="M12 0C5.37 0 0 5.37 0 12c0 5.3 3.44 9.8 8.2 11.38.6.11.82-.26.82-.58v-2.17c-3.34.73-4.04-1.61-4.04-1.61-.54-1.38-1.33-1.75-1.33-1.75-1.09-.74.08-.73.08-.73 1.2.09 1.84 1.24 1.84 1.24 1.07 1.83 2.8 1.3 3.49 1 .1-.78.42-1.3.76-1.6-2.67-.3-5.47-1.33-5.47-5.93 0-1.31.47-2.38 1.24-3.22-.12-.3-.54-1.52.12-3.17 0 0 1.01-.32 3.3 1.23a11.5 11.5 0 013-.4c1.02.01 2.04.14 3 .4 2.28-1.55 3.29-1.23 3.29-1.23.66 1.65.24 2.87.12 3.17.77.84 1.24 1.91 1.24 3.22 0 4.61-2.81 5.63-5.48 5.92.43.37.81 1.1.81 2.22v3.29c0 .32.21.7.82.58C20.56 21.8 24 17.3 24 12c0-6.63-5.37-12-12-12z"/></svg>
          Code
        </a>` : ""}
        ${p.liveUrl ? `<a href="${p.liveUrl}" target="_blank" rel="noopener" class="btn-ghost">
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" width="16" height="16"><path d="M18 13v6a2 2 0 01-2 2H5a2 2 0 01-2-2V8a2 2 0 012-2h6"/><polyline points="15 3 21 3 21 9"/><line x1="10" y1="14" x2="21" y2="3"/></svg>
          Live
        </a>` : ""}
      </div>
    </article>`;
  }).join("");
}

// ── Skills ────────────────────────────────────────────────────────
function renderSkills(skills) {
  const container = document.getElementById("skillsContainer");
  if (!container) return;

  if (!skills || !skills.length) {
    container.innerHTML = `<p class="empty-state">No skills data available.</p>`;
    return;
  }

  // Group by category
  const grouped = skills.reduce((acc, s) => {
    (acc[s.category] = acc[s.category] || []).push(s);
    return acc;
  }, {});

  container.innerHTML = Object.entries(grouped).map(([cat, items]) => `
    <div class="skill-group reveal">
      <h3 class="skill-category">${cat}</h3>
      <div class="skill-list">
        ${items.map(s => `
          <div class="skill-item">
            <div class="skill-meta">
              <span class="skill-name">${s.name}</span>
              <span class="skill-pct">${s.proficiency}%</span>
            </div>
            <div class="skill-bar">
              <div class="skill-fill" style="--target:${s.proficiency}%"></div>
            </div>
          </div>`).join("")}
      </div>
    </div>`).join("");

  // Trigger bar animations after render
  requestAnimationFrame(() => {
    document.querySelectorAll(".skill-fill").forEach((el, i) => {
      setTimeout(() => el.classList.add("animate"), i * 40);
    });
  });
}

// ── Experience ────────────────────────────────────────────────────
function renderExperience(experience) {
  const timeline = document.getElementById("timeline");
  if (!timeline) return;

  if (!experience || !experience.length) {
    timeline.innerHTML = `<p class="empty-state">No experience data available.</p>`;
    return;
  }

  timeline.innerHTML = experience.map((e, i) => {
    const start = new Date(e.startDate).getFullYear();
    const end = e.isCurrent ? "Present" : new Date(e.endDate).getFullYear();
    return `
    <div class="timeline-item reveal" style="animation-delay:${i * 100}ms">
      <div class="timeline-dot"></div>
      <div class="timeline-content">
        <div class="timeline-header">
          <h3>${e.role}</h3>
          <span class="timeline-period">${start} — ${end}</span>
        </div>
        <a ${e.companyUrl ? `href="${e.companyUrl}" target="_blank" rel="noopener"` : ""} class="company-name">
          ${e.company}${e.location ? ` · ${e.location}` : ""}
        </a>
        <p class="timeline-desc">${e.description}</p>
        ${e.isCurrent ? '<span class="badge current-badge">Current</span>' : ""}
      </div>
    </div>`;
  }).join("");
}

// ── Contact form ──────────────────────────────────────────────────
document.addEventListener("DOMContentLoaded", () => {
  const form = document.getElementById("contactForm");
  if (!form) return;

  form.addEventListener("submit", async (e) => {
    e.preventDefault();
    const btn    = form.querySelector("button[type=submit]");
    const status = document.getElementById("formStatus");

    const data = {
      name:    form.name.value.trim(),
      email:   form.email.value.trim(),
      subject: form.subject.value.trim(),
      message: form.message.value.trim()
    };

    btn.disabled    = true;
    btn.textContent = "Sending…";
    status.textContent = "";
    status.className   = "form-status";

    try {
      await API.contact.send(data);
      status.textContent = "✓ Message sent! I'll get back to you soon.";
      status.classList.add("success");
      form.reset();
    } catch (err) {
      status.textContent = `✗ ${err.message || "Something went wrong. Try again."}`;
      status.classList.add("error");
    } finally {
      btn.disabled    = false;
      btn.textContent = "Send Message";
    }
  });
});
