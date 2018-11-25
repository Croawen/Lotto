import React from "react"
import { View, StyleSheet, TouchableHighlight, Text } from "react-native"
import colors from "../helper/colors";

export default class BlueButton extends React.Component {
    static defaultProps = {
        name: "",
        onButtonPress: () => {},
        textSize: 20,
        fullWidth: false
    }

    render() {
        const bgColor = this.props.bgColor !== undefined ? this.props.bgColor : colors.colorAccent
        const underlayColor = this.props.underlayColor !== undefined ? this.props.underlayColor : colors.colorAccent

        widthComponent = this.props.fullWidth ? {flex: 1} : {width: this.props.buttonWidth}
        return (
            <View style={[styles.container, widthComponent, this.props.style]}>
                <TouchableHighlight
                        style={[styles.touchableContainer, widthComponent, { backgroundColor: bgColor }]}
                        onPress={this.props.onButtonPress}
                        underlayColor={underlayColor}>
                        <View style={styles.textContainer}>
                            <Text style={[styles.textStyle, { color: colors.white , fontSize: this.props.textSize}]}>
                                {this.props.name}
                            </Text>
                        </View>
                    </TouchableHighlight>
            </View>
        )
    }
}

const styles = StyleSheet.create({
    container: {
        flexDirection: "row"
    },
    touchableContainer: {
        height: 50,
        justifyContent: "center",
        borderRadius: 8
    },
    textContainer: {
        flexDirection: "row",
        justifyContent: "center",
        padding: 12
    },
    textStyle: {
        fontSize: 20,
        fontWeight: "bold",
        alignSelf:"center"
    }
})
