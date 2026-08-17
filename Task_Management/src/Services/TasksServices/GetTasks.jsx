const API_URL = import.meta.env.VITE_API_URL;

export async function getPendingTasks() {
    try {
        const response = await fetch(`${API_URL}/me/tasks/pending`, {
            method: "GET",
            credentials: "include"
        })
        if (!response.ok) {
            return;
        }
        return await response.json();
    }
    catch (error) {
        alert(error);
    }
}

export async function getCompletedTasks() {
    try {
        const response = await fetch(`${API_URL}/me/tasks/completed`, {
            method: "GET",
            credentials: "include"
        })
        if (!response.ok) {
            return;
        }

        return await response.json();
    }
    catch (error) {
        alert(error);
    }
}