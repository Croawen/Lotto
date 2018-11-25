import React from "react"
import { StyleSheet, View, TouchableOpacity, Text, Image } from "react-native"
import colors from "../helper/colors"


export default class LosePopUp extends React.Component {
    static defaultProps={
        playAgainClick:()=>{}
    }

    render() {
        return (
            <View style={styles.container}>
                <View style={styles.popUpContainer}>

                    <View style={{flexDirection:"row", marginTop:20, alignItems:"center"}}>
                        <Text style={[styles.text, {fontSize:12}]}>{`${this.props.coordinates.latitude.toFixed(4)} E`}</Text>
                        <Image source={require("../assets/redCircle.png")} style={{height:36, width:36, marginHorizontal:14}}/>
                        <Text style={[styles.text, {fontSize:12}]}>{`${this.props.coordinates.longitude.toFixed(4)} N`}</Text>   
                        
                    </View>                
                    <Text style={[styles.text, {fontSize:16, marginTop:40}]}>Próbuj dalej!</Text>
                    <Text style={[styles.text, {fontSize:12, marginTop:12, marginHorizontal:12, textAlign:"center"}]}>
                    Do następnej puli przechodzi 50 000zł
                    Następne losowanie za:</Text>
                    <Text style={[styles.text, {fontSize:30, marginTop:12}]}>{this.props.timeleft}</Text>
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

