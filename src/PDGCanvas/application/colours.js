const colours = {
    GREY: 'rgb(211,211,211)',
    BLACK: 'rgb(0,0,0)',
    RED: 'rgb(204,0,0)',
}

function constructColourMappingObject() {
    return {
        "Floor": colours.GREY,
        "Wall": colours.BLACK,
        "Obstacle": colours.RED
    }
}

module.exports = {
    setColour: (tileType) => (constructColourMappingObject())[tileType],

    getColourMapping: () => {
        return constructColourMappingObject()
    }
}