const colours = {
    GREY: 'rgb(211,211,211)',
    BLACK: 'rgb(0,0,0)',
    RED: 'rgb(204,0,0)',
    BROWN: 'rgb(135,64,8)',
}

function constructColourMappingObject() {
    return {
        "Floor": colours.GREY,
        "Wall": colours.BLACK,
        "Obstacle": colours.RED,
        "Door": colours.BROWN,
    }
}

module.exports = {
    setColour: (tileType) => (constructColourMappingObject())[tileType],

    getColourMapping: () => {
        return constructColourMappingObject()
    }
}