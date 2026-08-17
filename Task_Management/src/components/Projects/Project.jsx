import { Link } from "react-router-dom";

import Projects from "./Projects";
import "./Project.css";
function Project({ project, className }) {
    return (
        <Link to={`project/${project.id}`} className="project">
          <div className={className}>
              <div className="title-and-date">
                  <h4 className="project_title">{project.title}</h4>
                  <span className="date_project">{new Date(project.createdAt).toLocaleDateString("pt-br")}</span>
              </div>
                <p className="project-description">{project.description}</p>
          </div>
        </Link>
  );
}

export default Project;