import React from "react"
import { StyleSheet, View, TouchableOpacity, Text, Image } from "react-native"
import colors from "../helper/colors"


export default class WinPopUp extends React.Component {
    
    static defaultProps={
        playAgainClick:()=>{}
    }

    render() {
        return (
            <View style={styles.container}>
                <View style={styles.popUpContainer}>
                    <Text style={[styles.text, {fontSize:16, marginTop:36}]}>GRATULACJE!</Text>
                    <Image source={require("../assets/win.png")} style={{height:120, width:230, marginTop:16}}/>
                    <Text style={[styles.text, {fontSize:30, marginTop:12}]}>10 000zł</Text>
                    <Text style={[styles.text, {fontSize:16, position:"absolute", bottom:36, color:colors.colorAccent}]} onPress={this.props.playAgainClick}>Graj dalej</Text>
                </View>
            </View>
        )
    }
}

const styles = StyleSheet.create({
    container: {
        flex:1,
        position:"absolute",
        width:"100%",
        height:"100%",
        alignItems: "center",
        justifyContent:"center",
        backgroundColor:colors.bgPopUp
    },
    popUpContainer:{
        backgroundColor:colors.white,
        width:300,
        height:340,
        alignItems: "center",
    },
    text:{
        fontWeight: "bold",
        color: colors.textGrey
    }
})

