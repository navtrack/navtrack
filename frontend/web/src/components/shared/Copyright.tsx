export function Copyright() {
  return (
    <span>
      © {new Date().getFullYear()}{" "}
      <a
        href="https://codeagency.com"
        target="_blank"
        rel="noreferrer"
        title={import.meta.env.VITE_VERSION}>
        CodeAgency
      </a>
    </span>
  );
}
