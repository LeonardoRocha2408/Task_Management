export async function logout() {
    try {
        const response = await fetch(`${import.meta.env.VITE_API_URL}/logout`, {
            method: "POST",
            credentials: "include"
        })
        if (!response.ok) {
            return;
        }

        return response.ok;
    }
    catch (error) {
        alert(error);
    }
}