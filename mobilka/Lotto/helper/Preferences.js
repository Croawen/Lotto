import { AsyncStorage } from "react-native"

export default class Preferences {
    static USER = "USER"

    static put = async (key, value) => {
        try {
            var newValue = value
            if (typeof newValue === "object") {
                newValue = JSON.stringify(newValue)
            } else if (typeof typeof newValue !== "string") {
                newValue = value.toString()
            }
            await AsyncStorage.setItem(key, newValue)
        } catch (error) {

        }
    }

    static get = async key => {
        try {
            const value = await AsyncStorage.getItem(key)
            try{
                jsonValue = JSON.parse(value)
                return jsonValue
            }catch(error){
                return value
            }
        } catch (error) {

        }
    }

    static delete = async key => {
        try {
            await AsyncStorage.removeItem(key)
        } catch (error) {
            
        }
    }
}
