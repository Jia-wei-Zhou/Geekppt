cmake_minimum_required(VERSION 3.20)
project(CodeEvaluation)

set(CMAKE_CXX_STANDARD 20)

add_library(evaluation SHARED CodeEvaluation.cpp)

add_executable(CodeEvaluation Test.cpp)

target_link_libraries(CodeEvaluation evaluation)
