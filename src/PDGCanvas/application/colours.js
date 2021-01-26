const colours = {
    GREY: 'rgb(211,211,211)',
    BLACK: 'rgb(0,0,0)',
    RED: 'rgb(204,0,0)',
}

module.exports = {
    setColour: (tileType) => ({
        "Floor": colours.GREY,
        "Wall": colours.BLACK,
        "Obstacle": colours.RED
      })[tileType]
}