import { useEffect, useState } from "react";
import useAuth from "../.././AuthContext/useAuth";
import { getOwnedProjects, getParticipatingProjects } from "../../Services/ProjectServices/GetProjects";
import { getPendingTasks, getCompletedTasks } from "../../Services/TasksServices/GetTasks"
import Projects from "../.././components/Projects/Projects"
import { CreateProjectScreen } from "../../components/Projects/CreateProjectScreen"
import "./Home.css";
import { Sidebar } from "../../components/Sidebar/Sidebar";
function Home() {
    const { user } = useAuth()

    const [pendingTasks, setPendingTasks] = useState(0);
    const [completedTasks, setCompletedTasks] = useState(0);
    const [projects, setProjects] = useState([]);
    const [projectsParticipation, setProjectsParticipation] = useState([])
    const [showCreateProject, setShowCreateProject] = useState(false);

    useEffect(() => {
        async function loadScreen() {
            if (user === null) {
                return;
            }

            const pendingTasks = await getPendingTasks();
            setPendingTasks(pendingTasks);

            const completedTasks = await getCompletedTasks();
            setCompletedTasks(completedTasks);

            const userProjects = await getOwnedProjects()
            setProjects(userProjects);

            const userProjectsParticipation = await getParticipatingProjects();
            setProjectsParticipation(userProjectsParticipation);
        }
        loadScreen();
    }, [user])



    return (
        <div className="home_page-div">
        <Sidebar></Sidebar>
          <header>
              <div className="user_div">
                    <img src={user?.pathProfilePicture ?
                        `https://localhost:7161${user.pathProfilePicture}`
                        : "https://localhost:7161/uploads/profiles/default_user.webp"}
                    />
                    <div>
                        <p>{user?.userName}</p>
                        <p>{user?.email}</p>
                    </div>
              </div>
          </header>

          <main>
              <h1>{user ? `Welcome, ${user.userName}!` : "Loading"}</h1>

                <div className="current-resume">
                    <div className="tasks">
                        <img src="../../.././Images/task.png"
                            className="pending-tasks"/>
                        <div>
                            <h2>{pendingTasks}</h2>
                            <span>Tarefas pendentes</span>
                        </div>
                    </div>

                    <div className="tasks">
                        <img src="../../.././Images/completed-task.png"
                            className="completed-tasks"/>
                        <div>
                            <h2>{completedTasks}</h2>
                            <span>Tarefas completas</span>
                        </div>
                    </div>
                </div>


                <div className="user-projects">
                    <div className="create-project-header">
                        <span>Criar novo projeto</span>
                        <button className="create-new-project"
                            onClick={() => setShowCreateProject(true)}>+</button>
                    </div>


                    <Projects projects={projects}></Projects>
              </div>

              <div className="projects-participation">
                    <Projects projects={projectsParticipation}></Projects>
              </div>
            </main>
            {showCreateProject && <CreateProjectScreen onClose={() => setShowCreateProject(false)} />}
      </div>
  );
}

export default Home;