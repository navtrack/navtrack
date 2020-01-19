import React from "react";

export default function AdminFooter() {
    const date = new Date();
    const year = date.getFullYear();

    return (
        <footer className="text-sm">
            &copy; {year} <a href="https://navtrack.io" className="font-weight-bold" target="_blank" rel="noopener noreferrer">Navtrack</a>
        </footer>
    );
}