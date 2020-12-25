
export const ACTION_SET_APP_CONFIG = 'ACTION_SET_APP_CONFIG';// action identyfier

export const setAppConfig = (newAppConfig) => {

    return {
        type: ACTION_SET_APP_CONFIG,
        payload: newAppConfig
    };
}

