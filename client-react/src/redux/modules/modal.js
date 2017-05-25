const TOGGLE_MODAL = 'redux-example/modal/TOGGLE_MODAL';


const initialState = {
  modalState: false
};

export default function reducer(state = initialState, action) {
  switch (action.type) {
    case TOGGLE_MODAL:
      const actualState = state.modalState;
      const newState = actualState === false ? true : false;
      return {
        modalState: newState,
        action: action.action,
        title: action.title
       };
    default:
      return state;
  }
}

/*
* Actions
* * * * */
export function toggleModal(data) {
  return {
    type: TOGGLE_MODAL,
    ...data
  };
}
