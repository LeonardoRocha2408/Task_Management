import { useState } from "react";
import "./CreateProjectScreen.css"
export function CreateProjectScreen({ onClose }) {
    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const API_URL = import.meta.env.VITE_API_URL;

    async function createProject(e) {
        e.preventDefault();
        try {
            const response = await fetch(`${API_URL}/create_project`, {
                method: "POST",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    Title: title,
                    Description: description
                })
            })

            if (!response.ok) {
                return;
            }

            onClose();
        }
        catch (error) {
            alert(error);
        }
    }

    return (
        <div className="modal-overlay">
            <form onSubmit={createProject}>
                <button onClick={onClose} className="close-modal">x</button>

                <h2>Create new project</h2>
            
                <label htmlFor="title">Title</label>
                <input
                    id="title"
                    placeholder="Title"
                    maxLength={50}
                    value={title}
                    onChange={(e) => setTitle(e.target.value)}
                ></input>
                <p>{title.length}</p>

                <label htmlFor="description">Title</label>
                <textarea
                    id="description"
                    placeholder="Type a description"
                    maxLength={100}
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                ></textarea>
                <p>{description.length}</p>

                <button type="submit" className="create-project">Create</button>
            </form>
        </div>
    );
}