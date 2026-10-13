import { Link } from 'expo-router'
import React from 'react'
import { Image, Pressable, StyleSheet, Text } from 'react-native'

// Інтерфейс зображення
export interface Picture {
    Id: number
    Tags: string
    Data: string
    SentBy: string
    CreatedAt: string
}

// Карточка зображення в списку зображень
const card = ({ Id, Data, CreatedAt }: Picture) => {

    return (
        <Link
            href={{ pathname: '/infoPicture', params: { Id: String(Id) } }}
            asChild
        >
            <Pressable style={styles.button}>
                <Image
                    source={{ uri: `data:image/jpeg;base64,${Data}` }}
                    resizeMode='cover'
                    style={styles.image}
                />
                <Text style={styles.text}>Create: {new Date(CreatedAt).getFullYear()} year</Text>
            </Pressable>
        </Link>
    )
}

export default card;

const styles = StyleSheet.create({
    button: {
        width: '48%',
        backgroundColor: '#050018',
        padding: 10,
        borderRadius: 14,
        height: 170,
        margin: 4,
        borderWidth: 1.5,
        borderColor: '#2dd4bf',
        shadowColor: '#a855f7',
        shadowOffset: { width: 0, height: 0 },
        shadowOpacity: 0.9,
        shadowRadius: 12,
        elevation: 10
    },
    image: {
        height: '85%',
        width: '100%',
        margin: 0,
        borderRadius: 10
    },
    text: {
        color: '#c4b5fd',
        fontSize: 12,
        textAlign: 'left',
        marginTop: 4
    }
});