/*
This file in the main entry point for defining Gulp tasks and using Gulp
plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/
var gulp = require('gulp'),
gp_clean = require('gulp-clean'),
gp_concat = require('gulp-concat'),
gp_less = require('gulp-less'),
gp_sourcemaps = require('gulp-sourcemaps'),
gp_typescript = require('gulp-typescript'),
gp_uglify = require('gulp-uglify');
/// Define paths
var srcPaths = {
    app: ['scripts/app/main.ts', 'scripts/app/**/*.ts'],
    js: [
    'scripts/js/**/*.js',
    'node_modules/core-js/client/shim.min.js',
    'node_modules/zone.js/dist/zone.js',
    'node_modules/reflect-metadata/Reflect.js',
    'node_modules/systemjs/dist/system.src.js',
    'node_modules/typescript/lib/typescript.js',
    'node_modules/ng2-bootstrap/bundles/ng2-bootstrap.min.js',
    'node_modules/moment/moment.js',
    //'node_modules/bootstrap/dist/js/bootstrap.min.js'
    ],
    js_angular: [
    'node_modules/@angular/**'
    ],
    js_rxjs: [
    'node_modules/rxjs/**'
    ],
    less: [
    'Scripts/less/**/*.less',
    ],
    css: [
    'node_modules/bootstrap/dist/css/*min.css'
    ],
    fonts: [
    'node_modules/bootstrap/dist/fonts/**'
    ]
};
var destPaths = {
    app: 'wwwroot/app/',
    css: 'wwwroot/css/',
    fonts: 'wwwroot/fonts',
    js: 'wwwroot/js/',
    js_angular: 'wwwroot/js/@angular/',
    js_rxjs: 'wwwroot/js/rxjs/'
};
// Compile, minify and create sourcemaps all TypeScript files and place them to wwwroot/app, together with their js.map files.
gulp.task('app', ['app_clean'], function () {
    return gulp.src(srcPaths.app)
    .pipe(gp_sourcemaps.init())
    .pipe(gp_typescript(require('./tsconfig.json').compilerOptions))
    //.pipe(gp_uglify({ mangle: false }))
    .pipe(gp_sourcemaps.write('/'))
    .pipe(gulp.dest(destPaths.app));
});
// Delete wwwroot/app contents
gulp.task('app_clean', function () {
    return gulp.src(destPaths.app + "*", { read: false })
    .pipe(gp_clean({ force: true }));
});
// Copy all JS files from external libraries to wwwroot/js
gulp.task('js', ['js_clean'], function () {
    gulp.src(srcPaths.js_angular)
    .pipe(gulp.dest(destPaths.js_angular));
    gulp.src(srcPaths.js_rxjs)
    .pipe(gulp.dest(destPaths.js_rxjs));
    return gulp.src(srcPaths.js)
    // .pipe(gp_uglify({ mangle: false })) // disable uglify
    // .pipe(gp_concat('all-js.min.js')) // disable concat
    .pipe(gulp.dest(destPaths.js));
});
// Delete wwwroot/js contents
gulp.task('js_clean', function () {
    return gulp.src(destPaths.js + "*", { read: false })
    .pipe(gp_clean({ force: true }));
});
// Process all LESS files and output the resulting CSS in wwwroot/css
gulp.task('less', ['less_clean'], function () {
    return gulp.src(srcPaths.less)
    .pipe(gp_less())
    .pipe(gulp.dest(destPaths.css));
});
// Delete wwwroot/css contents
gulp.task('less_clean', function () {
    return gulp.src(destPaths.css + "*.*", { read: false })
    .pipe(gp_clean({ force: true }));
});

gulp.task('css', function () {
    gulp.src(srcPaths.fonts)
    .pipe(gulp.dest(destPaths.fonts));
    return gulp.src(srcPaths.css)
    .pipe(gulp.dest(destPaths.css));
});

// Watch specified files and define what to do upon file changes
gulp.task('watch', function () {
    gulp.watch([
    srcPaths.app,
    srcPaths.js,
    srcPaths.less],
    ['app', 'js', 'less']);
});
// Global cleanup task
gulp.task('cleanup', ['app_clean', 'js_clean', 'less_clean']);
// Define the default task so it will launch all other tasks
gulp.task('default', ['app', 'js', 'less']);