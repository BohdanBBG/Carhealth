

export const ACTION_CHANGE_FIRST_NAME = 'ACTION_CHANGE_FIRST_NAME';// action identyfier
export const ACTION_CHANGE_SECOND_NAME = 'ACTION_CHANGE_SECOND_NAME';


export const changeFirstName = (newFirstName) => {

    return {
        type: ACTION_CHANGE_FIRST_NAME,
        payload: newFirstName
    };
}

export const changeSecondName = (newSecondName) => ({ // OR we can use this syntax
    type: ACTION_CHANGE_SECOND_NAME,
    payload: newSecondName
});