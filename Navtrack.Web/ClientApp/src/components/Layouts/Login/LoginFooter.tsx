import React from "react";

export default function LoginFooter() {
    const date = new Date();
    const year = date.getFullYear();
    
    return (
        <footer className="py-5">
            <div className="container">
                <div className="row align-items-center justify-content-xl-between">
                    <div className="col-xl-6">
                        <div className="copyright text-center text-xl-left text-muted">
                            © {year} <a href="https://www.navtrack.io" className="font-weight-bold ml-1"
                                      target="_blank" rel="noopener noreferrer" >Navtrack</a>
                        </div>
                    </div>
                </div>
            </div>
        </footer>
    );
}