export const ScrollTableUtil = {
  scrollTableUp(newIndex: number) {
    let table = document.getElementById("scrollableTableContent");
    let row = document.getElementById(`scrollableTableRow-${newIndex}`);
    if (table && row) {
      let rowPos = newIndex * 30;
      let start = table.scrollTop;
      let end = start + 90;
      if (rowPos <= start || rowPos >= end) {
        table.scrollTop = rowPos;
      }
    }
  },

  scrollTableDown(newIndex: number) {
    let table = document.getElementById("scrollableTableContent");
    let row = document.getElementById(`scrollableTableRow-${newIndex}`);
    if (table && row) {
      let rowPos = newIndex * 30;
      let start = table.scrollTop;
      let end = start + 90;
      if (rowPos >= end || rowPos <= start) {
        table.scrollTop = rowPos - 90;
      }
    }
  }
};
