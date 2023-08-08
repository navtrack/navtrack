let showDebug = false;

export function log(...data: any[]) {
  if (showDebug) {
    console.log(...data);
  }
}

// @ts-ignore
window.toggleDebug = () => {
  showDebug = !showDebug;
  console.log("debug mode", showDebug);
};
