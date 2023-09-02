# project_H.U.L.K.
Segundo proyecto de programación de 1er año de Ciencias de la Computación en Matcom.

Contexto:

Para encontrar la orientación del proyecto ir a https://github.com/matcom/programming/blob/main/projects/hulk Ahi se define el conjunto del lenguaje que se implementa. Para encontrar la documentacion completa del lenguaje (el lenguaje completo, no el subconjunto), ir a https://github.com/matcom/hulk 

Cómo ejecutar el proyecto:

El .sln se encuentra dentro de la carpeta Hulk (por alguna razon se creo ahí). Esta solución consta de tres proyectos:

Hulk: una libreria de clases que contiene la logica del compilador.

Interface: la interfaz del lenguaje en forma de aplicacion de consola. Este es el proyecto que se deberia ejecutar si no se está en Windows.

FormInterface: la interfaz en forma de aplicacion de Windows Form, cumple la misma funcionalidad que Interface, solo que de forma un poco más amigable. Es recomendable iniciar este proyecto si se está usando Windows.

--------------------------------------------------------------------------------------------  

Branch: Modified Language

En esta branch es donde se guarda el compilador de H.U.L.K., pero en esta el lenguaje tiene ciertas diferencias:

-Se pueden declarar variables globales y usarlas en otra instruccion. (number x = 3; //por ejemplo)

-Se pueden declarar variables locales dentro de funciones sin necesidad de usar let-in

-Se pueden declarar funciones locales dentro de let-ins.

(las dos diferencias anteriores no tienen ninguna utilidad real pero estan hechas pensando en que estas se pudieran incluir en bloques de expresiones)

-Existen funciones que retornan void, como print().

-Para que la consola devuelva una instruccion se debe usar print() oblogatoriamente.
