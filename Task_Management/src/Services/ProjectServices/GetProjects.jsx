const API_URL = import.meta.env.VITE_API_URL;

export async function getOwnedProjects() {
    try {
        const response = await fetch(`${API_URL}/me/projects`, {
            method: "GET",
            credentials: "include"
        });
        if (!response.ok) {
            return;
        }
        return response.json();
    }
    catch (error) {
        alert(error);
    }
}

export async function getParticipatingProjects() {
    try {
        const response = await fetch(`${API_URL}/participing_projects`, {
            method: "GET",
            credentials: "include"
        });
        if (!response.ok) {
            return;
        }
        return response.json();
    }
    catch (error) {
        alert(error);
    }
}