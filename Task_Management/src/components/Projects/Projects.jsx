import Project from "./Project"
function Projects({ projects }) {
    return (
      <div>
          {projects.map((p) => (
              <Project
                  key={p.id}
                  project={p}
                  className={p.role}
              />))}
      </div>
  );
}

export default Projects;