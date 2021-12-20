import { useState } from "react";
import DeleteModal from "./DeleteModal";

export default {
  Default: () => {
    const [open, setOpen] = useState(true);

    return (
      <>
        <button className="border" onClick={() => setOpen(true)}>
          Open modal
        </button>
        <DeleteModal open={open} close={() => setOpen(false)}>
          <div className="bg-red-300">
            <div className="w-full">hello</div>
          </div>
        </DeleteModal>
      </>
    );
  }
};
