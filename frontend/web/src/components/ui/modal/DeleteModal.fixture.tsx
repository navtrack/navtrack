import { DeleteModal } from "./DeleteModal";

export default {
  Default: () => {
    return (
      <DeleteModal
        onConfirm={() => {
          console.log("confirm");
          return Promise.resolve();
        }}
        renderButton={(open) => (
          <button className="border" onClick={open}>
            Open modal
          </button>
        )}>
        <div className="bg-red-300">
          <div className="w-full">hello</div>
        </div>
      </DeleteModal>
    );
  }
};
