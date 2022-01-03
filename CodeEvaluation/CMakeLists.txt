cmake_minimum_required(VERSION 3.10)
project(CodeEvaluation)

set(CMAKE_CXX_STANDARD 20)

add_library(evaluation STATIC CodeEvaluation.cpp)
add_library(evaluation_shared SHARED CodeEvaluation.cpp)
set_target_properties(evaluation_shared PROPERTIES OUT_NAME evaluation)

add_executable(CodeEvaluation Test.cpp)

target_link_libraries(CodeEvaluation evaluation)
