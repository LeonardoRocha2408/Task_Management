import { NavLink } from "react-router-dom";
import { logout } from "../../Services/Logout"
import "./Sidebar.css";
export function Sidebar() {
    async function handleLogout() {
        const success = await logout();

        if (success) {
            window.location.href = "/login";
        }
    }
  return (
      <aside className="sidebar">
          <h2>TaskFlow</h2>
          <nav>
              <NavLink to="/home">🏠︎ Home</NavLink>
              <NavLink to="/projects">📝 Projects</NavLink>
          </nav>

          <button className="logout-button"
              onClick={handleLogout}>
              Logout
          </button>
      </aside>
  );
}

