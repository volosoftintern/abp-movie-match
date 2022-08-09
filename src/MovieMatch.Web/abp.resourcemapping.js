module.exports = {
    aliases: {
        "@node_modules": "./node_modules",
        "@libs": "./wwwroot/libs"
    },
    clean: [
        "@libs"
    ],
    mappings: {
        "@node_modules/twbs-pagination/**/*": "@libs/twbs-pagination/",
        "@node_modules/swiper/swiper-bundle.min.js": "@libs/swiper/js/",
        "@node_modules/swiper/swiper-bundle.min.css": "@libs/swiper/css/"
        
    }
};
