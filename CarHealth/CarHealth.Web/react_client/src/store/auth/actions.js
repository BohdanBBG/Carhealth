

export const ACTION_SET_APP_CONFIG = 'ACTION_SET_APP_CONFIG';// action identyfier
export const ACTION_SET_ACCESS_TOKEN = 'ACTION_SET_ACCESS_TOKEN';


export const setAppConfig = (newAppConfig) => {

    return {
        type: ACTION_SET_APP_CONFIG,
        payload: newAppConfig
    };
}

export const setAccessToken = (newAccessToken) => ({ // OR we can use this syntax
    type: ACTION_SET_ACCESS_TOKEN,
    payload: newAccessToken
});